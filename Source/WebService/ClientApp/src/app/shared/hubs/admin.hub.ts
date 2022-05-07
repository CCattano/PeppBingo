import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";

enum AdminEvents {
  EmitLatestActiveBoardID = 'EmitLatestActiveBoardID',
  TriggerSetActiveBoardCooldown = 'TriggerSetActiveBoardCooldown'
}

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
    this._registerInternalCooldownHandler();
  }

  /**
   * Register a callback function to act as handler for the
   * EmitLatestActiveBoardID event which broadcast a activeBoardID
   * representhing the latest active board that has been chosen by an admin
   * @param handler
   */
  public registerOnLatestActiveBoardIDHandler(handler: (activeBoardID: number) => void): void {
    this._hubConn.on(AdminEvents.EmitLatestActiveBoardID, handler);
  }

  /**
   * Register a callback function to act as handler for the
   * EmitLatestActiveBoardID event which broadcast a activeBoardID
   * representhing the latest active board that has been chosen by an admin
   * @param handler
   */
  public registerOnTriggerSetActiveBoardCooldown(handler: (timeRemaining?: number) => void): void {
    this._hubConn.off(AdminEvents.TriggerSetActiveBoardCooldown);
    if (this._eventCaptureDatetime) {
      const currentDatetime: Date = new Date();
      const cooldownExpireTime: Date =
        new Date(new Date(this._eventCaptureDatetime).setSeconds(this._eventCaptureDatetime.getSeconds() + 30));
      if (cooldownExpireTime > currentDatetime) {
        const timeRemaining: number =
          Math.round(Math.abs(cooldownExpireTime.getTime() - currentDatetime.getTime()) / 1000);
        handler(timeRemaining);
      }
    }
    this._hubConn.on(AdminEvents.TriggerSetActiveBoardCooldown, () => {
      this._eventCaptureDatetime = new Date();
      handler();
    });
  }

  public unregisterAllHandlers(): void {
    this._hubConn.off(AdminEvents.EmitLatestActiveBoardID);
    this._hubConn.off(AdminEvents.TriggerSetActiveBoardCooldown);
    this._registerInternalCooldownHandler();
  }

  private _registerInternalCooldownHandler(): void {
    this._hubConn.on(AdminEvents.TriggerSetActiveBoardCooldown, () =>
      this._eventCaptureDatetime = new Date());
  }
}
