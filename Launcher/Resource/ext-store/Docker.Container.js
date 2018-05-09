Ext.define('Docker.Container', {
    extend: 'Ext.data.Model',
    fields: [
        { name: 'Id', type: 'string' },
        { name: 'Name', type: 'string' },
        { name: 'Command', type: 'string' },
        { name: 'Created', type: 'date' },
        { name: 'Image', type: 'string' },
        { name: 'Status', type: 'string' }
    ]
});

Ext.create('Ext.data.Store', {
    id: 'Docker.Container',
    model: 'Docker.Container',
    autoLoad: false,
    remoteSort: true,
    proxy: {
        type: 'ajax',
        url: '/Launcher/Api/Container/List',
        pageParam: null,
        startParam: null,
        limitParam: null,
        noCache: false,
        simpleSortMode: true,
        sortParam: 'Sort',
        directionParam: 'Direction',
        reader: {
            type: 'json',
            root: 'data',
            //totalProperty: 'data.RecordCount',
            successProperty: 'success',
            messageProperty: 'message'
        },
        listeners: {
            exception: function (sender, response, operation, eOpts) {
                var message = 'network error';
                if (typeof (operation.error) == 'string')
                    message = operation.error;
                Ext.MessageBox.alert('get data failed.', message);
            }
        }
    }
});