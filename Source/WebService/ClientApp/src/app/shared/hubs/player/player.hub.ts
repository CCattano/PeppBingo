import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import {BingoSubmissionEvent} from './events/bingo-submission.event';

enum PlayerEvents {
  EmitLatestActiveBoardID = 'EmitLatestActiveBoardID',
  EmitBingoSubmission = 'EmitBingoSubmission'
}

@Injectable({
  providedIn: 'root'
})
export class PlayerHub {
  private _hubConn: HubConnection;

  /**
   * Represents the connection id of the HubConnection on the server.
   * The connection id will be null when the connection is either in
   * the disconnected state or if the negotiation step was skipped.
   */
  public get connectionID(): string {
    return this._hubConn?.connectionId;
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
        .withUrl('https://localhost:44339/playerHub')
        .withAutomaticReconnect()
        .build();
    await this._hubConn.start();
  }

  public registerEmitLatestActiveBoardIDHandler(handler: (activeBoardID: number) => void): void {
    this._hubConn.on(PlayerEvents.EmitLatestActiveBoardID, handler);
  }

  public registerEmitBingoSubmissionHandler(handler: (submission: BingoSubmissionEvent) => void): void {
    this._hubConn.on(PlayerEvents.EmitBingoSubmission, handler);
  }

  public unregisterAllHandlers(): void {
    this._hubConn.off(PlayerEvents.EmitLatestActiveBoardID);
    this._hubConn.off(PlayerEvents.EmitBingoSubmission);
    this._hubConn.stop();
    this._hubConn = null;
  }
}
