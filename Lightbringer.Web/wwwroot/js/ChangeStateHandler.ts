class ChangeStateHandler {
    changeState(hostId: number, type: string, daemon: string, state: string): void {

        const id = `${hostId}-${type}-${daemon}`;

        const spanElement: HTMLSpanElement = document.getElementById(id);
        if (spanElement == null) {
            console.log(`span not found: ${id}`);
            return;
        }

        const fasIElement = spanElement.getElementsByTagName("i")[0];

        let faChar: string;
        let color: string;

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

        // now change the start-stop button
        const startStopId = `start_stop-${id}`;
        const button = document.getElementById(startStopId) as HTMLButtonElement;
        if (button == null)
            return;

        let startStopColor = "btn-danger";
        let sign = "fa-stop";
        let nextAction = "stop";
        if (state === "Stopped") {
            startStopColor = "btn-success";
            sign = "fa-play";
            nextAction = "start";
        }

        button.setAttribute("next-action", nextAction);
        button.classList.remove("btn-danger", "btn-success");
        button.classList.add(startStopColor);

        const fasi = button.getElementsByTagName("i")[0];
        fasi.classList.remove("fa-play", "fa-stop");
        fasi.classList.add(sign);
    }
}