import {Injectable} from '@angular/core';
import {HubConnection, HubConnectionBuilder} from '@microsoft/signalr';

enum AdminEvents {
  LatestActiveBoardID = 'LatestActiveBoardID',
  StartSetActiveBoardCooldown = 'StartSetActiveBoardCooldown',
  StartResetAllBoardsCooldown = 'StartResetAllBoardsCooldown'
}

@Injectable({
  providedIn: 'root'
})
export class AdminHub {
  private _latestBoardIDEvtCaptureDT: Date;
  private _resetAllBoardsEvtCaptureDT: Date;
  private _hubConn: HubConnection;

  private static _handleInProgressCooldownEvent(lastEvtDateTime: Date, evtHandler: (timeRemaining?: number) => void): void {
    if (lastEvtDateTime) {
      const currentDatetime: Date = new Date();
      const cooldownExpireTime: Date =
        new Date(new Date(lastEvtDateTime).setSeconds(lastEvtDateTime.getSeconds() + 30));
      if (cooldownExpireTime > currentDatetime) {
        const timeRemaining: number =
          Math.round(Math.abs(cooldownExpireTime.getTime() - currentDatetime.getTime()) / 1000);
        evtHandler(timeRemaining);
      }
    }
  }

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
        .withUrl(`${window.location.origin}/adminHub`)
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
    this._hubConn.on(AdminEvents.LatestActiveBoardID, handler);
  }

  /**
   * Register a callback function to act as handler for the
   * EmitLatestActiveBoardID event which broadcast a activeBoardID
   * representhing the latest active board that has been chosen by an admin
   * @param handler
   */
  public registerOnStartSetActiveBoardCooldown(handler: (timeRemaining?: number) => void): void {
    this._hubConn.off(AdminEvents.StartSetActiveBoardCooldown);
    AdminHub._handleInProgressCooldownEvent(this._latestBoardIDEvtCaptureDT, handler);
    this._hubConn.on(AdminEvents.StartSetActiveBoardCooldown, () => {
      this._latestBoardIDEvtCaptureDT = new Date();
      handler();
    });
  }

  /**
   * Register a callback function to act as handler for the
   * EmitLatestActiveBoardID event which broadcast a activeBoardID
   * representhing the latest active board that has been chosen by an admin
   * @param handler
   */
  public registerOnStartResetAllBoardsCooldown(handler: (timeRemaining?: number) => void): void {
    this._hubConn.off(AdminEvents.StartResetAllBoardsCooldown);
    AdminHub._handleInProgressCooldownEvent(this._resetAllBoardsEvtCaptureDT, handler);
    this._hubConn.on(AdminEvents.StartResetAllBoardsCooldown, () => {
      this._resetAllBoardsEvtCaptureDT = new Date();
      handler();
    });
  }

  public unregisterAllHandlers(): void {
    this._hubConn.off(AdminEvents.LatestActiveBoardID);
    this._hubConn.off(AdminEvents.StartSetActiveBoardCooldown);
    this._hubConn.off(AdminEvents.StartResetAllBoardsCooldown);
    this._registerInternalCooldownHandler();
  }

  private _registerInternalCooldownHandler(): void {
    this._hubConn.on(AdminEvents.StartSetActiveBoardCooldown, () => this._latestBoardIDEvtCaptureDT = new Date());
    this._hubConn.on(AdminEvents.StartResetAllBoardsCooldown, () => this._resetAllBoardsEvtCaptureDT = new Date());
  }
}
