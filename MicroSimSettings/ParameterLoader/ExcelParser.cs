using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Text.RegularExpressions;
using System.IO;
using System.Globalization;
using System.ComponentModel;

namespace MicroSimSettings
{    
    public class ExcelParser
    {
        public static void Parse(string fileName, SimulationEnvironment simulationEnvironment)
        {
            Regex rx = new Regex("^lista:");
            Regex rx_tabla = new Regex("^tábla:");
            Regex rx_meta = new Regex("^meta:");
            List<string> lst_columnlist = Helpers.GenerateColumnList();

            using (SpreadsheetDocument sp = SpreadsheetDocument.Open(fileName, false))
            {
                int row_min = 0, row_max = 0; //Hack
                List<int> lst_regex_index = new List<int>();
                List<int> lst_tabla_index = new List<int>();
                List<int> lst_meta_index = new List<int>();                

                SharedStringTable st = sp.WorkbookPart.SharedStringTablePart.SharedStringTable;
                var shared_text = st.Elements().Where(x => x.LocalName == "si").ToList();
                var regex_text = (from r in shared_text where rx.IsMatch(r.InnerText.ToLower()) select r).ToList();
                var tabla_text = (from r in shared_text where rx_tabla.IsMatch(r.InnerText.ToLower()) select r).ToList();
                var meta_text = (from r in shared_text where rx_meta.IsMatch(r.InnerText.ToLower()) select r).ToList();
                
                foreach (var item in regex_text)
                {
                    lst_regex_index.Add(shared_text.IndexOf(item));
                }

                foreach (var item in tabla_text)
                {
                    lst_tabla_index.Add(shared_text.IndexOf(item));
                }

                //foreach (var item in tabla_text)
                //{
                //    lst_tabla_index.Add(shared_text.IndexOf(item));
                //}

                foreach (var item in meta_text)
                {
                    lst_meta_index.Add(shared_text.IndexOf(item));
                }

                IEnumerable<Sheet> sheets = sp.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                foreach (var loc_sheet in sheets)
                {
                    List<string> lst_regex_koord = new List<string>();
                    List<string> lst_tabla_koord = new List<string>();
                    List<string> lst_meta_koord = new List<string>();
                    string relationshipId = loc_sheet.Id.Value;
                    WorksheetPart worksheetPart = (WorksheetPart)sp.WorkbookPart.GetPartById(relationshipId);
                    string[] dimension = (worksheetPart.Worksheet.SheetDimension.GetAttribute("ref", "").Value).Split(':');
                    if (dimension.Length == 1) continue; //Empty worksheet -- skiping
                    for (int i = 0; i < 2; i++)
                    {
                        int k;
                        for (int z = 0; z < dimension[i].Length; z++)
                        {
                            if (Int32.TryParse(dimension[i][z].ToString(), out k) == true)
                            {
                                if (i == 0)
                                {
                                    row_min = Convert.ToInt32(dimension[i].Substring(z));
                                    break;
                                }
                                else
                                {
                                    row_max = Convert.ToInt32(dimension[i].Substring(z));
                                    break;
                                }
                            }
                        }
                    }
                    for (int rowindex = row_min; rowindex < row_max + 1; rowindex++)
                    {
                        Row activerow = worksheetPart.Worksheet.GetFirstChild<SheetData>().Elements<Row>().Where(x => x.RowIndex == rowindex).FirstOrDefault();
                        if (activerow != null)
                        {
                            foreach (Cell cell in activerow)
                            {
                                if (cell != null && cell.CellValue != null && cell.DataType != null && cell.DataType.HasValue)
                                {
                                    foreach (int index in lst_regex_index)
                                    {
                                        if (cell.DataType.Value == CellValues.SharedString && cell.InnerText == index.ToString()) lst_regex_koord.Add(cell.GetAttribute("r", "").Value);
                                    }
                                    foreach (int index in lst_tabla_index)
                                    {
                                        if (cell.DataType.Value == CellValues.SharedString && cell.InnerText == index.ToString()) lst_tabla_koord.Add(cell.GetAttribute("r", "").Value);
                                    }
                                    foreach (int index in lst_meta_index)
                                    {
                                        if (cell.DataType.Value == CellValues.SharedString && cell.InnerText == index.ToString()) lst_meta_koord.Add(cell.GetAttribute("r", "").Value);
                                    }
                                }
                            }
                        }
                    }
                    //-------------------------------------------------------------------------------------------

                    //Processing metadata
                    foreach (string koord in lst_meta_koord)
                    {
                        int rowindex = 0;
                        for (int i = 0; i < koord.Length; i++)
                        {
                            int k;
                            if (Int32.TryParse(koord.Substring(i), out k) == true)
                            {
                                rowindex = k;
                                break;
                            }
                        }
                        string column = koord.Substring(0, koord.Length - (rowindex.ToString()).Length).ToUpper();
                        bool tovabb = true;
                        Nómenklatúra nk = new Nómenklatúra();
                        List<string> lista = new List<string>();
                        Row current_row = worksheetPart.Worksheet.GetFirstChild<SheetData>().Elements<Row>().Where(r => r.RowIndex == rowindex).FirstOrDefault();

                        /*
                        if (current_row != null)
                        {
                            Cell current_cella = current_row.Elements<Cell>().Where(c => c.CellReference.Value.ToUpper() == column + (rowindex).ToString()).FirstOrDefault();
                            if (current_cella != null && current_cella.CellValue != null && current_cella.DataType != null && current_cella.DataType.HasValue)
                            {
                                int k = Convert.ToInt32(current_cella.InnerText);
                                //A nómenklatúra neve
                                lista.Add(shared_text[k].InnerText.Substring(6));
                                nk.Name = shared_text[k].InnerText.Substring(6);
                            }
                        }*/
                        while (tovabb)
                        {
                            Row next_row = worksheetPart.Worksheet.GetFirstChild<SheetData>().Elements<Row>().Where(r => r.RowIndex == rowindex + 1).FirstOrDefault();
                            if (next_row != null)
                            {
                                Cell next_cella = next_row.Elements<Cell>().Where(c => c.CellReference.Value.ToUpper() == column + (rowindex + 1).ToString()).FirstOrDefault();
                                if (next_cella != null && next_cella.CellValue != null && next_cella.DataType != null && next_cella.DataType.HasValue)
                                {
                                    Cell right_cella = next_row.Elements<Cell>().Where(c => c.CellReference.Value.ToUpper() == lst_columnlist[lst_columnlist.IndexOf(column) + 1] + (rowindex + 1).ToString()).FirstOrDefault();
                                    if (right_cella != null && right_cella.CellValue != null)// && right_cella.DataType != null && right_cella.DataType.HasValue) csak szám van beírva akkor nincs DataType
                                    {
                                        int k = Convert.ToInt32(next_cella.InnerText);
                                        int l = Convert.ToInt32(right_cella.InnerText);
                                        string key = shared_text[k].InnerText;
                                        string val = shared_text[l].InnerText;
                                        simulationEnvironment.MetaDictionary.Add(key, val);
                                        /*
                                        if (Convert.ToInt32(right_cella.InnerText) > 0) lista.Add(shared_text[k].InnerText + " = " + right_cella.InnerText);
                                        int value;
                                        int.TryParse(right_cella.InnerText, out value);
                                        nk.Elemek.Add(new NómentklatúraElem(shared_text[k].InnerText, value));
                                        */
                                    }                       
                                }
                                else tovabb = false;
                                rowindex++;
                            }
                            else tovabb = false;
                        }
                    }


                    //-------------------------------------------------------------------------------------------
                    //Processing nomenclatures
                    foreach (string koord in lst_regex_koord)
                    {
                        int rowindex = 0;
                        for (int i = 0; i < koord.Length; i++)
                        {
                            int k;
                            if (Int32.TryParse(koord.Substring(i), out k) == true)
                            {
                                rowindex = k;
                                break;
                            }
                        }
                        string column = koord.Substring(0, koord.Length - (rowindex.ToString()).Length).ToUpper();
                        bool tovabb = true;
                        Nómenklatúra nk = new Nómenklatúra();
                        List<string> lista = new List<string>();
                        Row current_row = worksheetPart.Worksheet.GetFirstChild<SheetData>().Elements<Row>().Where(r => r.RowIndex == rowindex).FirstOrDefault();

                        if (current_row != null)
                        {
                            Cell current_cella = current_row.Elements<Cell>().Where(c => c.CellReference.Value.ToUpper() == column + (rowindex).ToString()).FirstOrDefault();
                            if (current_cella != null && current_cella.CellValue != null && current_cella.DataType != null && current_cella.DataType.HasValue)
                            {
                                int k = Convert.ToInt32(current_cella.InnerText);
                                //A nómenklatúra neve
                                lista.Add(shared_text[k].InnerText.Substring(6));
                                nk.Name = shared_text[k].InnerText.Substring(6);
                            }
                        }
                        while (tovabb)
                        {
                            Row next_row = worksheetPart.Worksheet.GetFirstChild<SheetData>().Elements<Row>().Where(r => r.RowIndex == rowindex + 1).FirstOrDefault();
                            if (next_row != null)
                            {
                                Cell next_cella = next_row.Elements<Cell>().Where(c => c.CellReference.Value.ToUpper() == column + (rowindex + 1).ToString()).FirstOrDefault();
                                if (next_cella != null && next_cella.CellValue != null && next_cella.DataType != null && next_cella.DataType.HasValue)
                                {
                                    Cell right_cella = next_row.Elements<Cell>().Where(c => c.CellReference.Value.ToUpper() == lst_columnlist[lst_columnlist.IndexOf(column) + 1] + (rowindex + 1).ToString()).FirstOrDefault();
                                    if (right_cella != null && right_cella.CellValue != null)// && right_cella.DataType != null && right_cella.DataType.HasValue) csak szám van beírva akkor nincs DataType
                                    {
                                        int k = Convert.ToInt32(next_cella.InnerText);
                                        if (Convert.ToInt32(right_cella.InnerText) > 0) lista.Add(shared_text[k].InnerText + " = " + right_cella.InnerText);
                                        int value;
                                        int.TryParse(right_cella.InnerText,out value);
                                        nk.Elemek.Add(new NómentklatúraElem(shared_text[k].InnerText, value));
                                    }
                                    else
                                    {
                                        int k = Convert.ToInt32(next_cella.InnerText);
                                        lista.Add(shared_text[k].InnerText);
                                        nk.Elemek.Add(new NómentklatúraElem(shared_text[k].InnerText));
                                    }
                                }
                                else tovabb = false;
                                rowindex++;
                            }
                            else tovabb = false;
                        }
                        //Nómenklatúra nk = new Nómenklatúra();
                        nk.MetaName = (from x in simulationEnvironment.MetaDictionary where x.Key == nk.Name select x.Value).FirstOrDefault();

                        nk.listák = lista;
                        simulationEnvironment.Nómenklatúrák.Add(nk);
                    }

                    //Processing parameter tables
                    foreach (string koord in lst_tabla_koord)
                    {
                        ParameterTable vt = new ParameterTable();
                        simulationEnvironment.ValószínűségiTáblák.Add(vt);
                        int rowindex = 0;
                        for (int i = 0; i < koord.Length; i++)
                        {
                            int k;
                            if (Int32.TryParse(koord.Substring(i), out k) == true)
                            {
                                rowindex = k;
                                break;
                            }
                        }
                        int column = lst_columnlist.IndexOf(koord.Substring(0, koord.Length - (rowindex.ToString()).Length).ToUpper());
                        int first_column = lst_columnlist.IndexOf(koord.Substring(0, koord.Length - (rowindex.ToString()).Length).ToUpper());
                        Row row = worksheetPart.Worksheet.GetFirstChild<SheetData>().Elements<Row>().Where(r => r.RowIndex == rowindex).FirstOrDefault();
                        if (row != null)
                        {
                            Cell cell = row.Elements<Cell>().Where(c => c.CellReference.Value.ToUpper() == lst_columnlist[column] + (rowindex).ToString()).FirstOrDefault();
                            vt.Name = Helpers.StringHelper(shared_text[Convert.ToInt32(cell.InnerText)].InnerText.Substring(6));
                        }
                        rowindex++;
                        bool tovabb = true;
                        List<string> oszlop_nevek = new List<string>();
                        Row current_row = worksheetPart.Worksheet.GetFirstChild<SheetData>().Elements<Row>().Where(r => r.RowIndex == rowindex).FirstOrDefault();
                        if (current_row != null)
                        {
                            Cell current_cell = current_row.Elements<Cell>().Where(c => c.CellReference.Value.ToUpper() == lst_columnlist[column] + (rowindex).ToString()).FirstOrDefault();
                            while (current_cell != null && current_cell.InnerText != "")
                            {
                                int k = Convert.ToInt32(current_cell.InnerText);
                                oszlop_nevek.Add(shared_text[k].InnerText);
                                column++;
                                current_cell = current_row.Elements<Cell>().Where(c => c.CellReference.Value.ToUpper() == lst_columnlist[column] + (rowindex).ToString()).FirstOrDefault();
                            }
                            column = first_column;
                            /*
                            Cell current_cella = current_row.Elements<Cell>().Where(c => c.CellReference.Value.ToUpper() == column + (rowindex).ToString()).FirstOrDefault();
                            if (current_cella != null && current_cella.CellValue != null && current_cella.DataType != null && current_cella.DataType.HasValue)
                            {
                                int k = Convert.ToInt32(current_cella.InnerText);
                                oszlop_nevek.Add(shared_text[k].InnerText);
                            }
                            Cell cella1 = current_row.Elements<Cell>().Where(c => c.CellReference.Value.ToUpper() == lst_columnlist[lst_columnlist.IndexOf(column) + 1] + (rowindex).ToString()).FirstOrDefault();
                            if (current_cella != null && current_cella.CellValue != null && current_cella.DataType != null && current_cella.DataType.HasValue)
                            {
                                int k = Convert.ToInt32(cella1.InnerText);
                                oszlop_nevek.Add(shared_text[k].InnerText);
                            }
                            Cell cella2 = current_row.Elements<Cell>().Where(c => c.CellReference.Value.ToUpper() == lst_columnlist[lst_columnlist.IndexOf(column) + 2] + (rowindex).ToString()).FirstOrDefault();
                            if (current_cella != null && current_cella.CellValue != null && current_cella.DataType != null && current_cella.DataType.HasValue)
                            {
                                int k = Convert.ToInt32(cella2.InnerText);
                                oszlop_nevek.Add(shared_text[k].InnerText);
                            }
                            Cell cella3 = current_row.Elements<Cell>().Where(c => c.CellReference.Value.ToUpper() == lst_columnlist[lst_columnlist.IndexOf(column) + 3] + (rowindex).ToString()).FirstOrDefault();
                            if (current_cella != null && current_cella.CellValue != null && current_cella.DataType != null && current_cella.DataType.HasValue)
                            {
                                int k = Convert.ToInt32(cella3.InnerText);
                                oszlop_nevek.Add(shared_text[k].InnerText);
                            }*/
                            vt.OszlopNevek = oszlop_nevek; //*
                        }
                        
                        while (tovabb)
                        {
                            //int nem = 0;
                            //int ev = 0;
                            //int kor = 0;
                            Row next_row = worksheetPart.Worksheet.GetFirstChild<SheetData>().Elements<Row>().Where(r => r.RowIndex == rowindex + 1).FirstOrDefault();
                            bool nincstovabb = false;
                            if (next_row != null)
                            {
                                TáblázatSor ts = new TáblázatSor();
                                for (int i = 0; i < oszlop_nevek.Count; i++)
                                {
                                    Cell next_cell = next_row.Elements<Cell>().Where(c => c.CellReference == lst_columnlist[first_column + i] + (rowindex + 1).ToString()).FirstOrDefault();
                                    if (next_cell == null || next_cell.InnerText == "")
                                    {
                                        nincstovabb = true;
                                        break;
                                    } 
                                    if (i != oszlop_nevek.Count - 1)
                                    {
                                        int cell_value = Convert.ToInt32(next_cell.InnerText);
                                        ts.Indices.Add(cell_value);
                                    }
                                    else
                                    {
                                        ts.Value = double.Parse(next_cell.InnerText, CultureInfo.InvariantCulture);
                                    }
                                }
                                if (!nincstovabb) vt.AddLine(ts);
                                else tovabb = false;
                                rowindex++;
                            }
                            else tovabb = false;
                        }

                        //vt.Proceess();
                    }
                }
                //sheet vége
                /*
                shared_text -> shared stringek listája
                lst_regex_text -> azok az <si> tagek közé zárt xml elemek a shared string táblából, amelyek egyeztek a regexxel
                regex_index -> regex_text elemeinek a shared stringe táblában a sorszáma
                lst_regex_koord -> ahol azok a kifejezések elhelyezkednek a dokumentumban (nem a regex_text-el megegyező sorrendben)
                 */
            }
            foreach (ParameterTable vt in simulationEnvironment.ValószínűségiTáblák)
            {
                    for (int i = 0; i < vt.OszlopNevek.Count; i++)
                    {
                        bool előfordult = false;
                        for (int j = 0; j < simulationEnvironment.Nómenklatúrák.Count; j++)
                        {
                            if (vt.OszlopNevek[i] == simulationEnvironment.Nómenklatúrák[j].listák[0])
                            {
                                vt.nómenklatúrák.Add(simulationEnvironment.Nómenklatúrák[j]);
                                előfordult = true;
                            }
                        }
                        if (!előfordult) vt.nómenklatúrák.Add(null);
                    }
            }


            //foreach (Nómenklatúra nk in simulationEnvironment.Nómenklatúrák)
            //{
            //    foreach (ValószínűségiTábla vt in simulationEnvironment.ValószínűségiTáblák)
            //    {
            //        bool előfordult = false;
            //        for (int i = 0; i < vt.OszlopNevek.Count; i++)
            //        {
            //            if (vt.OszlopNevek[i] == nk.listák[0])
            //            {
            //                vt.nómenklatúrák.Add(nk);
            //                előfordult = true;
            //            }
            //        }
            //        if (!előfordult) vt.nómenklatúrák.Add(null);
            //    }
            //}


        }


    }

    [Serializable()]
    public class TáblázatSor
    {
        public List<int> Indices;
        public double Value;
        public TáblázatSor() 
        {
            Indices = new List<int>();
        }
    }    
}
