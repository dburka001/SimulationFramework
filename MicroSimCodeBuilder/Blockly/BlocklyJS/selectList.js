Blockly.Blocks['selectList'] = {
init: function() {
        this.setMutator(new Blockly.Mutator(['select_item']));
        this.itemCount_ =  1;
        this.setInputsInline(false);
        this.appendDummyInput()
            .setAlign(Blockly.ALIGN_RIGHT)
            .appendField("List");            
        this.updateShape_();
        this.setOutput(true, null);
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
        var containerBlock = workspace.newBlock('select_item_container');
        containerBlock.initSvg();
        var connection = containerBlock.getInput('STACK').connection;
        for (var i = 0; i < this.itemCount_; i++) {
            var itemBlock = workspace.newBlock('select_item');
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

Blockly.Blocks['select_item_container'] = {
    init: function() {
        this.appendDummyInput()
            .setAlign(Blockly.ALIGN_RIGHT)
            .appendField("Item list");
        this.appendStatementInput('STACK');
        this.setColour(120);
        this.setTooltip('');
    }
};

Blockly.Blocks['select_item'] = {
    init: function() {
        this.appendDummyInput()
            .setAlign(Blockly.ALIGN_RIGHT)
            .appendField("item");
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(120);
        this.setTooltip('');
        this.contextMenu = false;
    }
};


Blockly.CSharp['selectList'] = function (block) {    
    var code = '';
    for (var i = 0; i < this.itemCount_; i++) {
        var currentName = this.getFieldValue('NAME' + i);
        code += currentName + ' = ' + Blockly.CSharp.valueToCode(this, 'ADD' + i, Blockly.CSharp.ORDER_COMMA) + ', ';
    }
    return [code, 1];  
};