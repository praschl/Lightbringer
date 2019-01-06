var ChangeStateHandler = /** @class */ (function () {
    function ChangeStateHandler() {
    }
    ChangeStateHandler.prototype.changeState = function (hostId, type, daemon, state) {
        var id = hostId + "-" + type + "-" + daemon;
        var spanElement = document.getElementById(id);
        if (spanElement == null) {
            console.log("span not found: " + id);
            return;
        }
        var fasIElement = spanElement.getElementsByTagName("i")[0];
        var faChar;
        var color;
        switch (state) {
            case "Running":
                faChar = "fa-play";
                color = "badge-success";
                break;
            case "Stopped":
                faChar = "fa-stop";
                color = "badge-danger";
                break;
            default:
                faChar = "fa-question";
                color = "badge-secondary";
                break;
        }
        spanElement.classList.remove("badge-success", "badge-danger", "badge-secondary");
        spanElement.classList.add(color);
        spanElement.title = state;
        fasIElement.classList.remove("fa-play", "fa-stop", "fa-question");
        fasIElement.classList.add(faChar);
    };
    return ChangeStateHandler;
}());
//# sourceMappingURL=ChangeStateHandler.js.map