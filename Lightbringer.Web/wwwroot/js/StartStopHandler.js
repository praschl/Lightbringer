var StartStopHandler = /** @class */ (function () {
    function StartStopHandler() {
    }
    StartStopHandler.prototype.getDaemonId = function (event) {
        var button = event.currentTarget;
        var id = button.getAttribute("daemon-id");
        return id;
    };
    StartStopHandler.prototype.start = function (event) {
        var fullId = this.getDaemonId(event);
        console.log("start click " + fullId);
    };
    StartStopHandler.prototype.stop = function (event) {
        var fullId = this.getDaemonId(event);
        console.log("stop click " + fullId);
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