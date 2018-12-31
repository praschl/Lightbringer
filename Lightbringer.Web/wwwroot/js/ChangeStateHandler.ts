class ChangeStateHandler {
    changeState(hostId: number, type: string, daemon: string, state: string): void {
        console.log(hostId, type, daemon, state);
    }
}