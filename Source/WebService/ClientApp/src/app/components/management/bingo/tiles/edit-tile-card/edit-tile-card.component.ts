import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { faEdit, faSave, faTrash, IconDefinition } from '@fortawesome/free-solid-svg-icons';
import { AdminApi } from '../../../../../shared/api/admin.api';
import { BoardTileDto } from '../../../../../shared/dtos/board-tile.dto';
import { ToastService } from '../../../../../shared/service/toast.service';
import { BoardTileVM } from '../../viewmodel/board-data.viewmodel';
import { EditTileForm } from './edit-tile.form';

@Component({
  selector: 'app-edit-tile-card',
  templateUrl: './edit-tile-card.component.html',
  styleUrls: ['../shared-tile-styles.scss']
})
export class EditTileCardComponent implements OnInit {
  /**
   * The tile to be edited by this component
   */
  @Input()
  public tile: BoardTileVM;

  /**
   * The index of the tile currently being edited
   */
  @Input()
  public index: number;

  /**
   * The ID of the board this tile is for
   */
  @Input()
  public boardIdForCreate: number;

  /**
   * Event emitted when changes are canceled.
   *
   * Emits the index of the tile that is no longer being edited
   */
  @Output()
  public readonly cancelClick: EventEmitter<number> = new EventEmitter<number>();

  /**
   * Event emitted when a save has occurred successfully
   *
   * Emits the index of the tile that has just been updated successfully
   */
  @Output()
  public readonly saveSuccess: EventEmitter<number> = new EventEmitter<number>();

  /**
   * The Reactive Form that holds the data changes to be applied to the tile provided
   */
  public readonly _tileForm: EditTileForm = new EditTileForm();

  /**
   * Fontawesome icons used in the template
   */
  public readonly icons: { [icon: string]: IconDefinition; } = {
    'faSave': faSave,
    'faEdit': faEdit,
    'faTrash': faTrash
  };

  constructor(
    private _adminApi: AdminApi,
    private _toastService: ToastService
  ) {
  }

  /**
   * @inheritdoc
   */
  public ngOnInit(): void {
    this._tileForm.form.reset(this.tile);
  }

  /**
   * Event handler for the Save click event
   */
  public async _onSaveClick(): Promise<void> {
    if (!this._tileForm.form.valid) {
      this._tileForm.form.markAllAsTouched();
      return;
    }
    if (this._tileForm.form.pristine)
      this.tile.editing = false;;
    let newTile: BoardTileDto;
    if (this.tile.isNew) {
      const tileToCreate: BoardTileDto = new BoardTileDto();
      tileToCreate.text = this._tileForm.controls.text.value;
      tileToCreate.isActive = this._tileForm.controls.isActive.value;
      newTile = await this._createTile(tileToCreate);
    } else {
      const tileToCreate: BoardTileDto = {
        ...this.tile,
        text: this._tileForm.controls.text.value,
        isActive: this._tileForm.controls.isActive.value
      };
      newTile = await this._updateTile(tileToCreate);
    }
    if (!newTile) return;
    Object.keys(newTile).forEach((key: string) =>
      (this.tile as any)[key] = (newTile as any)[key]);
    this.tile.isNew = false;
    this.tile.editing = false;
    this.saveSuccess.emit(this.index);
  }

  /**
   * Event handler for when the discard changes button is clicked
   */
  public _onDiscardClick(): void {
    this.tile.editing = false;
    this.cancelClick.emit(this.index);
  }

  private async _createTile(tile: BoardTileDto): Promise<BoardTileDto> {
    return await this._adminApi.createNewBoardTile(this.boardIdForCreate, tile).catch(() => {
      this._toastService.showDangerToast({
        header: 'An Error Occurred!',
        body: 'We couldn\'t create your tile. Please try again.',
        ttlMs: 3000
      });
      return null;
    });
  }

  private async _updateTile(tile: BoardTileDto): Promise<BoardTileDto> {
    return await this._adminApi.updateBoardTile(tile).catch(() => {
      this._toastService.showDangerToast({
        header: 'An Error Occurred!',
        body: 'We couldn\'t update this tile. Please try again.',
        ttlMs: 3000
      });
      return null;
    });
  }
}
