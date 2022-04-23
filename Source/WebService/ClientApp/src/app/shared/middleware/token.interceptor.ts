import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { TokenService } from '../service/token.service';


@Injectable({
  providedIn: 'root'
})
export class TokenInterceptor implements HttpInterceptor {
  constructor(private _tokenService: TokenService) {
  }

  public intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    let httpReq: HttpRequest<any> = req.clone();
    if (this._tokenService.haveToken) {
      httpReq = req.clone({
        headers: req.headers.append(TokenService.accessTokenName, this._tokenService.token)
      });
    }
    return next.handle(httpReq);
  }
}
