import {Injectable} from "@angular/core";
import {HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from "@angular/common/http";
import {catchError, Observable} from "rxjs";

@Injectable()
export class ErrorHttpInterceptor implements HttpInterceptor {
  intercept(req: HttpRequest<any>, next: HttpHandler):
    Observable<HttpEvent<any>> {
    return next.handle(req).pipe(catchError(async e => {
      if (e instanceof HttpErrorResponse) {
        this.showError(e.statusText);
      }
      throw e;
    }));
  }
}
