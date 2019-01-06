class OverviewHandler {
    
    private _hostId: number;

    constructor(hostId: number) {
        this._hostId = hostId;
    }

    // TODO: get daemon from button custom attribute "daemon-name"
    async toggle(event: MouseEvent, daemon: string) {

        const button = (event.currentTarget) as HTMLButtonElement;
        if (!button)
            throw "could not get expected button from event.currentTarget";

        const subscribeData = JSON.stringify(
            {
                hostId: this._hostId,
                name: daemon
            });

        const result = await fetch("/api/subscribe",
                {
                    method: "POST",
                    headers: {
                        "Accept": "application/json",
                        "Content-Type": "application/json"
                    },
                    body: subscribeData
                })
            .then(r => r.json())
            .catch(r => console.log(r));

        button.getElementsByTagName("i")[0].classList.toggle("fa-check");

        if (result) {
            button.style.opacity = "1";
        } else {
            button.style.opacity = "0.5";
        }
    }
}