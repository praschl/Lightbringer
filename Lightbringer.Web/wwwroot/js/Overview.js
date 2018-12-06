var OverviewHandler = /** @class */ (function () {
    function OverviewHandler(host) {
        this._host = host;
    }
    OverviewHandler.prototype.toggle = function (service) {
        var addButton = document.querySelector("#add-" + service);
        var removeButton = document.querySelector("#remove-" + service);
        if (addButton.style.display === "none") {
            addButton.style.display = "inline-block";
            removeButton.style.display = "none";
        }
        else {
            addButton.style.display = "none";
            removeButton.style.display = "inline-block";
        }
    };
    return OverviewHandler;
}());
//# sourceMappingURL=Overview.js.map