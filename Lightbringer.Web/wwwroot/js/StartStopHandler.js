var StartStopHandler = /** @class */ (function () {
    function StartStopHandler() {
    }
    StartStopHandler.prototype.getDaemonId = function (event) {
        var button = event.currentTarget;
        var id = button.getAttribute("daemon-id");
        return id;
    };
    StartStopHandler.prototype.parse = function (id) {
        return id.split("-");
    };
    StartStopHandler.prototype.send = function (command, host, type, daemon) {
        fetch(Site.root + "/api/managedaemon/" + command + "?id=" + host + "&type=" + type + "&daemon=" + daemon)
            .catch(function (r) { return console.log(r); });
    };
    StartStopHandler.prototype.startStop = function (event) {
        var fullId = this.getDaemonId(event);
        var parsed = this.parse(fullId);
        var button = event.currentTarget;
        var action = button.getAttribute("next-action");
        this.send(action, parsed[0], parsed[1], parsed[2]);
    };
    StartStopHandler.prototype.initialize = function () {
        var _this = this;
        var buttons = document.getElementsByName("start-stop-btn");
        buttons.forEach(function (b) {
            b.addEventListener("click", function (e) { return _this.startStop(e); });
        });
    };
    return StartStopHandler;
}());
//# sourceMappingURL=StartStopHandler.js.map