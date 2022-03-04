import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { HttpClient } from '@angular/common/http';

export function padNumber(num: number, size: number) {
    let s = num.toString();
    if (s.length < size)
        return "0" + s;
    else
        return s;
}

export function createTranslateLoader(http: HttpClient) {
    return new TranslateHttpLoader(http, './assets/languages/', '.json');
}