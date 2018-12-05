

class OverviewHandler {
    addClick(addButton: HTMLButtonElement, removeButton: HTMLButtonElement, service: any) {
        console.log(`add: ${service}`);
    }

    removeClick(addButton: HTMLButtonElement, removeButton: HTMLButtonElement, service: string) {
        console.log(`remove: ${service}`);
    }
}

var overview = new OverviewHandler();