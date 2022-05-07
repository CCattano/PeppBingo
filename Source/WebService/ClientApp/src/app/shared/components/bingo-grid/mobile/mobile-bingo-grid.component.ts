import {Component} from '@angular/core';
import {BingoGridAbstract} from '../bingo-grid-abstract';

@Component({
  selector: 'app-mobile-bingo-grid',
  templateUrl: './mobile-bingo-grid.component.html',
  styleUrls: [
    '../bingo-grid.styles.scss',
  ]
})
export class MobileBingoGridComponent extends BingoGridAbstract {
}
