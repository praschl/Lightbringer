class OverviewHandler {

    _host: string;

    constructor(host: string) {
        this._host = host;
    }

    toggle(service: string) {
        const addButton: HTMLButtonElement = document.querySelector(`#add-${service}`);
        const removeButton: HTMLButtonElement = document.querySelector(`#remove-${service}`);

        if (addButton.style.display === "none") {
            addButton.style.display = "inline-block";
            removeButton.style.display = "none";
        } else {
            addButton.style.display = "none";
            removeButton.style.display = "inline-block";
        }
    }
}
