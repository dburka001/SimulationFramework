Blockly.Blocks['select'] = {
init: function() {
        this.setMutator(new Blockly.Mutator(['select_group_item']));
        this.itemCount_ =  0;
        this.setInputsInline(false);
        this.appendDummyInput()
            .setAlign(Blockly.ALIGN_RIGHT)
            .appendField(new Blockly.FieldTextInput("NameOfSelection"), "name")            
            .appendField("Select")
            .appendField(new Blockly.FieldDropdown([["Sum", "Sum"], ["Count", "Count"], ["Average", "Avg"]]), "type");
        this.appendValueInput("property")
            .setCheck("Person_properties_get")
            .setAlign(Blockly.ALIGN_RIGHT)
            .appendField("of");
        this.appendValueInput("where")
            .setCheck("Boolean")
            .setAlign(Blockly.ALIGN_RIGHT)
            .appendField("where");
        this.updateShape_();
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(120);
        this.setTooltip('Select values to save at the end of the simulation');
    },

    mutationToDom: function () {
        var container = document.createElement('mutation');
        container.setAttribute('items', this.itemCount_);
        return container;
    },

    domToMutation: function (xmlElement) {
        this.itemCount_ = parseInt(xmlElement.getAttribute('items'), 10);
        this.updateShape_();
    },

    decompose: function (workspace) {
        var containerBlock = workspace.newBlock('select_group_container');
        containerBlock.initSvg();
        var connection = containerBlock.getInput('STACK').connection;
        for (var i = 0; i < this.itemCount_; i++) {
            var itemBlock = workspace.newBlock('select_group_item');
            itemBlock.initSvg();
            connection.connect(itemBlock.previousConnection);
            connection = itemBlock.nextConnection;
        }
        return containerBlock;
    },

    compose: function (containerBlock) {
        var itemBlock = containerBlock.getInputTargetBlock('STACK');
        // Count number of inputs.
        var connections = [];
        while (itemBlock) {
            connections.push(itemBlock.valueConnection_);
            itemBlock = itemBlock.nextConnection &&
                itemBlock.nextConnection.targetBlock();
        }
        // Disconnect any children that don't belong.
        for (var i = 0; i < this.itemCount_; i++) {
            var connection = this.getInput('ADD' + i).connection.targetConnection;
            if (connection && connections.indexOf(connection) == -1) {
                connection.disconnect();
            }
        }
        this.itemCount_ = connections.length;
        this.updateShape_();
        // Reconnect any child blocks.
        for (var i = 0; i < this.itemCount_; i++) {
            Blockly.Mutator.reconnect(connections[i], this, 'ADD' + i);
        }
    },

    updateShape_: function() {
        // Add new inputs.
        for (var i = 0; i < this.itemCount_; i++) {            
            if (!this.getInput('ADD' + i)) {                                                   
                var input = this.appendValueInput('ADD' + i);
                if (i == 0) {
                    input.appendField("groupby")
                         .setAlign(Blockly.ALIGN_RIGHT);
                }
                input.appendField(new Blockly.FieldTextInput('Param' + i), 'NAME' + i)
                     .setAlign(Blockly.ALIGN_RIGHT);
            }
        }
        // Remove deleted inputs.
        while (this.getInput('ADD' + i)) {
            this.removeInput('ADD' + i);
            i++;
        }
    } 
};

Blockly.Blocks['select_group_container'] = {
    init: function() {
        this.appendDummyInput()
            .setAlign(Blockly.ALIGN_RIGHT)
            .appendField("Group list");
        this.appendStatementInput('STACK');
        this.setColour(120);
        this.setTooltip('');
    }
};

Blockly.Blocks['select_group_item'] = {
    init: function() {
        this.appendDummyInput()
            .setAlign(Blockly.ALIGN_RIGHT)
            .appendField("group");
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(120);
        this.setTooltip('');
        this.contextMenu = false;
    }
};


Blockly.CSharp['select'] = function (block) {    
    var name = block.getFieldValue('name');
    var dropdown_type = block.getFieldValue('type');
    var value_where = Blockly.CSharp.valueToCode(block, 'where', Blockly.CSharp.ORDER_ATOMIC);
    if (value_where != '') value_where = '.Where(p => ' + value_where + ')';
    var value_property = Blockly.CSharp.valueToCode(block, 'property', Blockly.CSharp.ORDER_ATOMIC);
    var indent = 0;
    var linqCode = '';
    linqCode += 'List<ResultItem> select' + name + ' = ';
    if (value_property == '') value_property = 'p';
    if (this.itemCount_ > 0) {
        linqCode += 'Population';
        linqCode += value_where;
        linqCode += '.GroupBy(p => new { ';
        for (var i = 0; i < this.itemCount_; i++) {
            var currentName = this.getFieldValue('NAME' + i);
            linqCode += currentName + ' = (' + Blockly.CSharp.valueToCode(this, 'ADD' + i, Blockly.CSharp.ORDER_COMMA) + ')';
            if (this.itemCount_ != i + 1) { linqCode += ', '; }
        }
        linqCode += ' }).Select(g => new ResultItem() { Year = Year, Key = g.Key, Value = g.Select(p => ' + value_property + ').' + dropdown_type + '() }).ToList();';
    }
    else {
        linqCode += 'new List<ResultItem>();\nselect' + name + '.Add(new ResultItem() { Year = Year, Value = Population' + value_where + '.Select(p => ' + value_property + ').' + dropdown_type + '() });';
    }

  
    var code = [];
    code.push(linqCode);
    code.push('Result ' + name + ' = ResultList.Find((x) => x.Name.Equals(\"' + name + '\"));');
    code.push('if(' + name + ' == null) { ' + name + ' = new Result(\"' + name + '\"); ResultList.Add(' + name + '); }');
    code.push(name + '.AddSelectResult(select' + name + ');');
    code.push('');
    return code.join('\n');
};

/*
Blockly.CSharp['select'] = function (block) {
    var name = block.getFieldValue('name');
    var dropdown_type = block.getFieldValue('type');
    var value_where = Blockly.CSharp.valueToCode(block, 'where', Blockly.CSharp.ORDER_ATOMIC);
    var value_property = Blockly.CSharp.valueToCode(block, 'property', Blockly.CSharp.ORDER_ATOMIC);
    var indent = 0;
    var code = [];
    for (var i = 0; i < this.itemCount_; i++) {
        code.push('List<object> paramList' + i + ';');
        var propName = Blockly.CSharp.valueToCode(this, 'ADD' + i, Blockly.CSharp.ORDER_COMMA);
        propName = propName.substring(propName.indexOf("p.") + 2);
        var endPosition = propName.indexOf(" ");
        if(endPosition > 0) propName = propName.substring(0, endPosition);                
        code.push('paramList' + i + ' =  Population.Select((x) => x.' + propName + ').Distinct().ToList<object>();');
        code.push('if(paramList' + i + ' == null)');
        code.push('{');
        code.push('}');
    }
    code.push('Result ' + name + ' = ResultList.Find((x) => x.Name.Equals(\"' + name + '\"));');
    code.push('if(' + name + ' == null) { ' + name + ' = new Result(\"' + name + '\", \"Year\"); ResultList.Add(' + name + '); }');    
    return code.join('\n');
};
*/