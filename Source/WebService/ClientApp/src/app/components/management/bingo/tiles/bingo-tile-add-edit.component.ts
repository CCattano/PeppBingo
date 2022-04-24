import { Component } from '@angular/core';
import { faPlusCircle, faEdit, faTrash, IconDefinition } from '@fortawesome/free-solid-svg-icons';
import { BoardTileVM } from './viewmodels/board-tile.viewmodel';

@Component({
  templateUrl: './bingo-tile-add-edit.component.html',
  styleUrls: ['./bingo-tile-add-edit.component.scss']
})
export class BingoTileAddEditComponent {
  public _tiles: BoardTileVM[] = new Array(30).fill(0).map((_, i) => ({
    tileID: i,
    text: `This is tile #${i + 1}`,
    isActive: true,
    createdDateTime: new Date(new Date().toISOString()),
    createdBy: 1,
    createdByName: 'TORTUGAN_TORRES',
    modDateTime: new Date(new Date().toISOString()),
    modBy: 1,
    modByName: 'TORTUGAN_TORRES',
    editing: false
  } as BoardTileVM));

  /**
   * Fontawesome icons used in the template
   */
  public readonly icons: { [icon: string]: IconDefinition; } = {
    'faPlusCircle': faPlusCircle,
    'faEdit': faEdit,
    'faTrash': faTrash
  };
}
