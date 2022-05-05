import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';

enum PlayerEvents {
  EmitLatestActiveBoardID = 'EmitLatestActiveBoardID'
}

@Injectable({
  providedIn: 'root'
})
export class PlayerHub {
  private _hubConn: HubConnection;

  /**
   * Connect to the SignalR hub associated with this service
   *
   * If a connection has already been established,
   * invoking this does nothing.
   */
  public async connect(): Promise<void> {
    if (this._hubConn) return;
    this._hubConn =
      new HubConnectionBuilder()
        .withUrl('https://localhost:44339/playerHub')
        .withAutomaticReconnect()
        .build();
    await this._hubConn.start();
  }

  public registerEmitLatestActiveBoardIDHandler(handler: (activeBoardID: number) => void): void {
    this._hubConn.on(PlayerEvents.EmitLatestActiveBoardID, (activeBoardID: number) => handler(activeBoardID));
  }

  public unregisterAllHandlers(): void {
    this._hubConn.off(PlayerEvents.EmitLatestActiveBoardID);
    this._hubConn.stop();
    this._hubConn = null;
  }
}
