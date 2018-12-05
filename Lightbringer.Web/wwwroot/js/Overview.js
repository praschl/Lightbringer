var OverviewHandler = /** @class */ (function () {
    function OverviewHandler() {
    }
    OverviewHandler.prototype.addClick = function (addButton, service) {
        console.log(service);
        addButton.style.display = "none";
        var removeButton = $("#remove-" + service)[0];
        console.log(removeButton);
        removeButton.style.display = "inline-block";
    };
    OverviewHandler.prototype.removeClick = function (removeButton, service) {
        console.log(service);
        removeButton.style.display = "none";
        var addButton = $("#add-" + service)[0];
        addButton.style.display = "inline-block";
    };
    return OverviewHandler;
}());
var overview = new OverviewHandler();
//# sourceMappingURL=Overview.js.map