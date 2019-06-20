Blockly.Blocks['selectItem'] = {
init: function() {        
        this.setInputsInline(true);
        this.appendDummyInput()
            .setAlign(Blockly.ALIGN_RIGHT) 
            .appendField(new Blockly.FieldDropdown([["Sum", "Sum"], ["Count", "Count"], ["Average", "Average"]]), "type");
        this.appendValueInput("property")
            .setAlign(Blockly.ALIGN_RIGHT)
            .appendField("of");
        this.setOutput(true, null);
        this.setColour(120);
        this.setTooltip('Decide how to calculate selected properties');
    }
};

Blockly.CSharp['selectItem'] = function (block) {
    var dropdown_type = block.getFieldValue('type');
    var value_property = Blockly.CSharp.valueToCode(block, 'property', Blockly.CSharp.ORDER_ATOMIC);
    var code = 'g.' + dropdown_type + '(p => ' + value_property + ')'; 
    return [code, 1];    
};