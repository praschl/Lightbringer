class OverviewHandler {

    _serviceHostId: number;

    constructor(serviceHostId: number) {
        this._serviceHostId = serviceHostId;
    }

    async toggle(service: string) {
        const addButton: HTMLButtonElement = document.querySelector(`#add-${service}`);
        const removeButton: HTMLButtonElement = document.querySelector(`#remove-${service}`);

        const result = await fetch(`/api/subscribe?serviceHostId=${this._serviceHostId}&service=${service}`)
            .then(r => r.json());

        if (result) {
            addButton.style.display = "none";
            removeButton.style.display = "inline-block";
        } else {
            addButton.style.display = "inline-block";
            removeButton.style.display = "none";
        }
    }
}