Ext.define('Docker.Image', {
    extend: 'Ext.data.Model',
    fields: [
        { name: 'id', type: 'string' },
        { name: 'repoTags', type: 'string' },
        { name: 'repo', type: 'string' },
        { name: 'tag', type: 'string' },
        { name: 'created', type: 'date' },
        { name: 'sharedSize', type: 'number' },
        { name: 'size', type: 'number' },
        { name: 'virtualSize', type: 'number' }
    ]
});

Ext.create('Ext.data.Store', {
    id: 'Docker.Image',
    model: 'Docker.Image',
    autoLoad: false,
    remoteSort: true,
    proxy: {
        type: 'rest',
        url: '/api/Image',
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