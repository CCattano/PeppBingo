import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";

@Injectable({
  providedIn: 'root'
})
export class AdminHub {
  private _eventCaptureDatetime: Date;
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
        .withUrl('https://localhost:44339/adminHub')
        .withAutomaticReconnect()
        .build();
    await this._hubConn.start();
  }

  /**
   * Register a callback function to act as handler for the
   * EmitLatestActiveBoardID event which broadcast a activeBoardID
   * representhing the latest active board that has been chosen by an admin
   * @param handler
   */
  public registerOnLatestActiveBoardIDHandler(handler: (activeBoardID: number) => void) {
    this._hubConn.on('EmitLatestActiveBoardID', (activeBoardID: number) => handler(activeBoardID));
  }

  /**
   * Register a callback function to act as handler for the
   * EmitLatestActiveBoardID event which broadcast a activeBoardID
   * representhing the latest active board that has been chosen by an admin
   * @param handler
   */
  public registerOnTriggerSetActiveBoardCooldown(handler: (timeRemaining: number) => void) {
    debugger;
    const currentDatetime: Date = new Date();
    if (this._eventCaptureDatetime) {
      const cooldownExpireTime: Date =
        new Date(this._eventCaptureDatetime.setSeconds(this._eventCaptureDatetime.getSeconds() + 30));
      if (cooldownExpireTime > currentDatetime) {
        const timeRemaining: number =
          Math.round(Math.abs(this._eventCaptureDatetime.getTime() - currentDatetime.getTime()) / 1000);
        handler(timeRemaining);
      }
    }
    this._hubConn.on('TriggerSetActiveBoardCooldown', () => {
      this._eventCaptureDatetime = new Date();
      handler(null);
    });
  }

  public unregisterAllHandlers(): void {
    this._hubConn.off('EmitLatestActiveBoardID');
    this._hubConn.off('TriggerSetActiveBoardCooldown');
    this._hubConn.stop();
    this._hubConn = null;
  }
}
