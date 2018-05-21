Ext.define('Docker.Container', {
    extend: 'Ext.data.Model',
    fields: [
        { name: 'id', type: 'string' },
        { name: 'name', type: 'string' },
        { name: 'command', type: 'string' },
        { name: 'created', type: 'date' },
        { name: 'image', type: 'string' },
        { name: 'status', type: 'string' }
    ]
});

Ext.create('Ext.data.Store', {
    id: 'Docker.Container',
    model: 'Docker.Container',
    autoLoad: false,
    remoteSort: true,
    proxy: {
        type: 'rest',
        url: '/api/v1/Container',
        pageParam: null,
        startParam: null,
        limitParam: null,
        noCache: false,
        simpleSortMode: true,
        sortParam: 'Sort',
        directionParam: 'Direction',
        listeners: {
            exception: function (sender, response, operation, eOpts) {
                var message = 'network error';
                if (typeof (operation.error) == 'string')
                    message = operation.error;
                else
                    message = response.status + ' ' + response.statusText + '<br/>' + response.responseText;
                Ext.MessageBox.alert('Error', message);
            }
        }
    }
});