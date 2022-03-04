import { OwlDateTimeIntl } from 'ng-pick-datetime';
import { Injectable } from '@angular/core';

export class GlobalConstants {

    public static enableLoading() {
        let loadingOverlay = document.querySelector('.loading-overlay') as HTMLElement;
        loadingOverlay.style.visibility = 'visible';
    }

    public static disableLoading() {
        let loadingOverlay = document.querySelector('.loading-overlay') as HTMLElement;
        loadingOverlay.style.visibility = 'hidden';
    }
}

@Injectable()
export class BGIntl extends OwlDateTimeIntl {
    /** A label for the cancel button */
    cancelBtnLabel = 'Откажи';

    /** A label for the set button */
    setBtnLabel = 'Запази';

    /** A label for the range 'from' in picker info */
    rangeFromLabel = 'От';

    /** A label for the range 'to' in picker info */
    rangeToLabel = 'До';
};