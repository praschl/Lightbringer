class ChangeStateHandler {
    changeState(hostId: number, type: string, daemon: string, state: string): void {

        const id = `${hostId}-${type}-${daemon}`;
        const spanElement: HTMLSpanElement = document.getElementById(id);
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
    }
}