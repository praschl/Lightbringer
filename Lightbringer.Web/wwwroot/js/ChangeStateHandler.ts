class ChangeStateHandler {
    changeState(hostId: number, type: string, daemon: string, state: string): void {

        const id = `${hostId}-${type}-${daemon}`;

        const spanElement: HTMLSpanElement = document.getElementById(id);
        if (spanElement == null) {
            // if we happen up here, a service which we do not display was changed
            return;
        }

        const fasIElement = spanElement.getElementsByTagName("i")[0];

        // BEGIN DaemonState display
        let faChar: string;
        let color: string;
        let animation = false;

        switch (state) {
        case "Running":
            faChar = "fa-play";
            color = "badge-success";
            break;
        case "Stopped":
            faChar = "fa-stop";
            color = "badge-danger";
            break;
        case "StartPending":
            faChar = "fa-forward";
            color = "badge-primary";
            animation = true;
            break;
        case "StopPending":
            faChar = "fa-eject";
            color = "badge-primary";
            animation = true;
            break;
        default:
            faChar = "fa-question";
            color = "badge-secondary";
            break;
        }

        spanElement.classList.remove("badge-success", "badge-danger", "badge-secondary", "badge-primary");
        spanElement.classList.add(color);
        spanElement.title = state;

        fasIElement.classList.remove("fa-play", "fa-stop", "fa-question", "fa-forward", "fa-eject", "faa-flash", "animated");
        fasIElement.classList.add(faChar);
        if (animation)
            fasIElement.classList.add("faa-flash", "animated");

        // BEGIN DaemonButtons
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