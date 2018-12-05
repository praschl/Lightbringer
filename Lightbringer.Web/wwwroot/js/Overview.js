var OverviewHandler = /** @class */ (function () {
    function OverviewHandler() {
    }
    OverviewHandler.prototype.addClick = function (addButton, removeButton, service) {
        console.log("add: " + service);
    };
    OverviewHandler.prototype.removeClick = function (addButton, removeButton, service) {
        console.log("remove: " + service);
    };
    return OverviewHandler;
}());
var overview = new OverviewHandler();
//# sourceMappingURL=Overview.js.map