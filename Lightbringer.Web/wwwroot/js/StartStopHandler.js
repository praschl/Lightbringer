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
    StartStopHandler.prototype.start = function (event) {
        var fullId = this.getDaemonId(event);
        var parsed = this.parse(fullId);
        fetch("api/managedaemon/start?id=" + parsed[0] + "&type=" + parsed[1] + "&daemon=" + parsed[2])
            .then(function (r) { return r.json(); })
            .catch(function (r) { return console.log(r); });
    };
    StartStopHandler.prototype.stop = function (event) {
        var fullId = this.getDaemonId(event);
        var parsed = this.parse(fullId);
        fetch("api/managedaemon/stop?id=" + parsed[0] + "&type=" + parsed[1] + "&daemon=" + parsed[2])
            .then(function (r) { return r.json(); })
            .catch(function (r) { return console.log(r); });
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