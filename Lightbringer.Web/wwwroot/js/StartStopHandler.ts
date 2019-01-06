class StartStopHandler {

    private getDaemonId(event: MouseEvent): string {
        const button = event.currentTarget as HTMLButtonElement;
        const id = button.getAttribute("daemon-id");
        return id;
    }



    start(event: MouseEvent) {
        const fullId = this.getDaemonId(event);
        console.log(`start click ${fullId}`);
    }

    stop(event: MouseEvent) {
        const fullId = this.getDaemonId(event);
        console.log(`stop click ${fullId}`);
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
