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
        fetch("api/managedaemon/" + command + "?id=" + host + "&type=" + type + "&daemon=" + daemon)
            .then(function (r) { return r.json(); })
            .catch(function (r) { return console.log(r); });
    };
    StartStopHandler.prototype.start = function (event) {
        var fullId = this.getDaemonId(event);
        var parsed = this.parse(fullId);
        this.send("start", parsed[0], parsed[1], parsed[2]);
    };
    StartStopHandler.prototype.stop = function (event) {
        var fullId = this.getDaemonId(event);
        var parsed = this.parse(fullId);
        this.send("stop", parsed[0], parsed[1], parsed[2]);
    };
    StartStopHandler.prototype.initialize = function () {
        var _this = this;
        var startButtons = document.getElementsByName("startButton");
        var stopButtons = document.getElementsByName("stopButton");
        startButtons.forEach(function (b) {
            b.addEventListener("click", function (e) { return _this.start(e); });
        });
        stopButtons.forEach(function (b) {
            b.addEventListener("click", function (e) { return _this.stop(e); });
        });
    };
    return StartStopHandler;
}());
//# sourceMappingURL=StartStopHandler.js.map