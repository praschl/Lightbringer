var ChangeStateHandler = /** @class */ (function () {
    function ChangeStateHandler() {
    }
    ChangeStateHandler.prototype.changeState = function (hostId, type, daemon, state) {
        var id = hostId + "-" + type + "-" + daemon;
        var spanElement = document.getElementById(id);
        if (spanElement == null) {
            // if we happen up here, a service which we do not display was changed
            return;
        }
        var fasIElement = spanElement.getElementsByTagName("i")[0];
        // BEGIN DaemonState display
        var faChar;
        var color;
        var animation = false;
        switch (state) {
            case "Running":
                faChar = "fa-play";
                color = "badge-success";
                break;
            case "Stopped":
                faChar = "fa-stop";
                color = "badge-danger";
                break;
            case "StartPending":
                faChar = "fa-forward";
                color = "badge-primary";
                animation = true;
                break;
            case "StopPending":
                faChar = "fa-eject";
                color = "badge-primary";
                animation = true;
                break;
            default:
                faChar = "fa-question";
                color = "badge-secondary";
                break;
        }
        spanElement.classList.remove("badge-success", "badge-danger", "badge-secondary", "badge-primary");
        spanElement.classList.add(color);
        spanElement.title = state;
        fasIElement.classList.remove("fa-play", "fa-stop", "fa-question", "fa-forward", "fa-eject", "faa-flash", "animated");
        fasIElement.classList.add(faChar);
        if (animation)
            fasIElement.classList.add("faa-flash", "animated");
        // BEGIN DaemonButtons
        var startStopId = "start_stop-" + id;
        var button = document.getElementById(startStopId);
        if (button == null)
            return;
        var startStopColor = "btn-danger";
        var sign = "fa-stop";
        var nextAction = "stop";
        if (state === "Stopped") {
            startStopColor = "btn-success";
            sign = "fa-play";
            nextAction = "start";
        }
        button.setAttribute("next-action", nextAction);
        button.classList.remove("btn-danger", "btn-success");
        button.classList.add(startStopColor);
        var fasi = button.getElementsByTagName("i")[0];
        fasi.classList.remove("fa-play", "fa-stop");
        fasi.classList.add(sign);
    };
    return ChangeStateHandler;
}());
//# sourceMappingURL=ChangeStateHandler.js.map