import {Component} from '@angular/core';
import {BingoGridAbstract} from '../bingo-grid-abstract';

@Component({
  selector: 'app-desktop-bingo-grid',
  templateUrl: './desktop-bingo-grid.component.html',
  styleUrls: [
    '../bingo-grid.styles.scss',
    './desktop-bingo-grid.component.scss'
  ]
})
export class DesktopBingoGridComponent extends BingoGridAbstract {
}
