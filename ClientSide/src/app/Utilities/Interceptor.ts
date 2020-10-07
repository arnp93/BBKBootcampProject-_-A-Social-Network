import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { DomainName } from './PathTools';

export class Interceptor implements HttpInterceptor{

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        
        const request = req.clone({
            url : DomainName + req.url
        });

        return next.handle(request);
    }
    
}