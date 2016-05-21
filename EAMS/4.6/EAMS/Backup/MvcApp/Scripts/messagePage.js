/// <reference path="jquery-1.7.2.js" />
/// <reference path="knockout-2.1.0.js" />

function MessageViewModel() {
    var self = this;
    //SELECT autoid,title,content,sender,receiver,markRead,sendDate,receivDate,contenType,[module]  FROM dbo.Message

    function MessageItem(root, autoid, title, content,sender,receiver,markRead,contenType,module) {
        var self = this,
            updating = false;

        //参数模型
        self.id = autoid;
        self.title = ko.observable(title);
        self.content = ko.observable(content);
        self.sender = ko.observable(sender);
        self.receiver = ko.observable(receiver);
        self.markRead = ko.observable(markRead);
        self.contenType= ko.observable(contenType);
        self.module = ko.observable(module);

        //方法
        self.remove = function () {
            root.sendDelete(self);
        };

        self.update = function (title, content) {
            updating = true;
            self.title(title);
            self.content(content);
            updating = false;
        };

        self.subscribe(function () {
            if (!updating) {
                root.sendUpdate(self);
            }
        });
    };

    self.addItemTitle = ko.observable("");
    self.items = ko.observableArray();

    self.add = function (id, title, content) {
        self.items.push(new MessageItem(self, id, title, content));
    };

    self.remove = function (id) {
        self.items.remove(function (item) { return item.id === id; });
    };

    self.update = function (id, title, contect) {
        var oldItem = ko.utils.arrayFirst(self.items(), function (i) { return i.id === id; });
        if (oldItem) {
            oldItem.update(title, contect);
        }
    };

    self.sendCreate = function () {
        $.ajax({
            url: "/api/message",
            data: { 'Title': self.addItemTitle(), 'markRead': false },
            type: "POST"
        });

        self.addItemTitle("");
    };

    self.sendDelete = function (item) {
        $.ajax({
            url: "/api/message/" + item.id,
            type: "DELETE"
        });
    }

    self.sendUpdate = function (item) {
        $.ajax({
            url: "/api/message/" + item.id,
            data: { 'Title': item.title(), 'markRead': item.markRead() },
            type: "PUT"
        });
    };
};

$(function () {
    var viewModel = new MessageViewModel(),
        hub = $.connection.todoMessage;

    ko.applyBindings(viewModel);

    hub.addItem = function (item) {
        viewModel.add(item.ID, item.Title, item.contect);
    };
    hub.deleteItem = function (id) {
        viewModel.remove(id);
    };
    hub.updateItem = function (item) {
        viewModel.update(item.ID, item.Title, item.content);
    };

    $.connection.hub.start();

    $.get("/api/message", function (items) {
        $.each(items, function (idx, item) {
            viewModel.add(item.ID, item.Title, item.contect);
        });
    }, "json");
});
