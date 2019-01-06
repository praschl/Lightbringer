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
            .then(r => r.json())
            .catch(r => console.log(r));
    }
    
    start(event: MouseEvent) {
        const fullId = this.getDaemonId(event);
        const parsed = this.parse(fullId);

        this.send("start", parsed[0], parsed[1], parsed[2]);
    }

    stop(event: MouseEvent) {
        const fullId = this.getDaemonId(event);
        const parsed = this.parse(fullId);

        this.send("stop", parsed[0], parsed[1], parsed[2]);
    }

    initialize() {
        const startButtons = document.getElementsByName("startButton");
        const stopButtons = document.getElementsByName("stopButton");

        startButtons.forEach(b => {
            b.addEventListener("click", e => this.start(e));
        });
        stopButtons.forEach(b => {
            b.addEventListener("click", e => this.stop(e));
        });
    }
}
