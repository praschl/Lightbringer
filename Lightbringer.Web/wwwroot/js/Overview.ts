class OverviewHandler {
    addClick(addButton: HTMLButtonElement, service: string) {
        console.log(service);

        addButton.style.display = "none";

        const removeButton = $(`#remove-${service}`)[0];
        console.log(removeButton);
        removeButton.style.display = "inline-block";
    }

    removeClick(removeButton: HTMLButtonElement, service: string) {
        console.log(service);
        
        removeButton.style.display = "none";

        const addButton = $(`#add-${service}`)[0];
        addButton.style.display = "inline-block";
    }
}

var overview = new OverviewHandler();
