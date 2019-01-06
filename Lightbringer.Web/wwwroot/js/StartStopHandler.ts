class StartStopHandler {

    private getDaemonId(event: MouseEvent): string {
        const button = event.currentTarget as HTMLButtonElement;
        const id = button.getAttribute("daemon-id");
        return id;
    }

    private parse(id: string): string[] {
        return id.split("-");
    }

    start(event: MouseEvent) {
        const fullId = this.getDaemonId(event);
        const parsed = this.parse(fullId);

        fetch(`api/managedaemon/start?id=${parsed[0]}&type=${parsed[1]}&daemon=${parsed[2]}`)
            .then(r => r.json())
            .catch(r => console.log(r));
    }

    stop(event: MouseEvent) {
        const fullId = this.getDaemonId(event);
        const parsed = this.parse(fullId);

        fetch(`api/managedaemon/stop?id=${parsed[0]}&type=${parsed[1]}&daemon=${parsed[2]}`)
            .then(r => r.json())
            .catch(r => console.log(r));
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
