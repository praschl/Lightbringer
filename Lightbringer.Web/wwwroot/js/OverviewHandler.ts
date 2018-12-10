class OverviewHandler {
    
    _serviceHostId: number;

    constructor(serviceHostId: number) {
        this._serviceHostId = serviceHostId;
    }
    
    async toggle(event: MouseEvent, service: string) {

        const button = (event.currentTarget) as HTMLButtonElement;
        if (!button)
            throw "could not get expected button from event.currentTarget";

        const result = await fetch(`/api/subscribe?serviceHostId=${this._serviceHostId}&service=${service}`)
            .then(r => r.json());

        button.getElementsByTagName("i")[0].classList.toggle("fa-check");

        if (result) {
            button.style.opacity = "1";
        } else {
            button.style.opacity = "0.5";
        }
    }
}