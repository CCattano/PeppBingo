import {fromEvent, Observable, Subscription} from 'rxjs';
import {debounceTime, distinctUntilChanged, filter, map, tap} from 'rxjs/operators';
import {Component, OnDestroy, OnInit} from '@angular/core';
import {BingoGridAbstract} from './bingo-grid-abstract';

enum BreakpointsEnum {
  xs = 0,
  sm = 576,
  md = 768,
  lg = 992,
  xl = 1200,
  xxl = 1400
}

@Component({
  selector: 'app-bingo-grid',
  templateUrl: './bingo-grid-container.component.html',
  styleUrls: ['./bingo-grid.styles.scss']
})
export class BingoGridContainerComponent extends BingoGridAbstract implements OnInit, OnDestroy {
  /**
   * Enum reference captures for use in template
   */
  public readonly breakpointsEnum: typeof BreakpointsEnum = BreakpointsEnum;
  /**
   * The current breakpoint the page width is associated with
   *
   * Used to determine when to display mobile vs. desktop bingo board
   */
  public _currBreakpoint: BreakpointsEnum;
  /**
   * Bool flag that indicated we are in a mobile
   * width and in a landscape orientation
   */
  public _isMobileLandscape: boolean;

  /**
   * Observable constructed from the window.onresize event
   */
  private _resizeEvent$: Observable<Event> = fromEvent(window, 'resize');
  /**
   * Subscription to the resize event observable
   */
  private _resizeSub: Subscription;

  private static _calcViewport(): BreakpointsEnum {
    const width: number = document.documentElement.clientWidth;
    if (width >= BreakpointsEnum.xxl)
      return BreakpointsEnum.xxl;
    else if (width >= BreakpointsEnum.xl)
      return BreakpointsEnum.xl;
    else if (width >= BreakpointsEnum.lg)
      return BreakpointsEnum.xl;
    else if (width >= BreakpointsEnum.md)
      return BreakpointsEnum.md;
    else if (width >= BreakpointsEnum.sm)
      return BreakpointsEnum.sm;
    else
      return BreakpointsEnum.xs;
  }

  private static _calcIsMobileLandscape(breakpoint: BreakpointsEnum): boolean {
    return breakpoint < BreakpointsEnum.lg && window.matchMedia('(orientation: landscape)').matches;
  }

  /**
   * @inheritDoc
   */
  public ngOnInit(): void {
    this._currBreakpoint = BingoGridContainerComponent._calcViewport();
    this._isMobileLandscape = BingoGridContainerComponent._calcIsMobileLandscape(this._currBreakpoint);
    this._resizeSub = this._initResizeEventPipeline().subscribe();
  }

  /**
   * @inheritdoc
   */
  public ngOnDestroy(): void {
    this._resizeSub?.unsubscribe();
    this._resizeSub = null;
  }

  private _initResizeEventPipeline(): Observable<void> {
    return this._resizeEvent$.pipe(
      debounceTime(250),
      distinctUntilChanged(),
      map(() => BingoGridContainerComponent._calcViewport()),
      filter((breakpoint: BreakpointsEnum) =>
        this._currBreakpoint !== breakpoint),
      tap((breakpoint: BreakpointsEnum) => {
        this._currBreakpoint = breakpoint;
        this._isMobileLandscape = BingoGridContainerComponent._calcIsMobileLandscape(breakpoint);
      }),
      map(() => null)
    );
  }
}
