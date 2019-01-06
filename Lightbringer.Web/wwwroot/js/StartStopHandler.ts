class StartStopHandler {

    private getDaemonId(event: MouseEvent): string {
        const button = event.currentTarget as HTMLButtonElement;
        const id = button.getAttribute("daemon-id");
        return id;
    }

    private parse(id: string): string[] {
        return id.split("-");
    }

    private send(command: string, host: string, type: string, daemon: string) {
        fetch(`api/managedaemon/${command}?id=${host}&type=${type}&daemon=${daemon}`)
            .catch(r => console.log(r));
    }
    
    startStop(event: MouseEvent) {
        const fullId = this.getDaemonId(event);
        const parsed = this.parse(fullId);

        const button = event.currentTarget as HTMLButtonElement;
        const action = button.getAttribute("next-action");

        this.send(action, parsed[0], parsed[1], parsed[2]);
    }

    initialize() {
        const buttons = document.getElementsByName("start-stop-btn");

        buttons.forEach(b => {
            b.addEventListener("click", e => this.startStop(e));
        });
    }
}
