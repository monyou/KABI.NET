import { MatPaginatorIntl } from '@angular/material/paginator';
import { TranslateService } from '@ngx-translate/core';

export class CustomMatPaginatorIntl extends MatPaginatorIntl {
    translate: TranslateService;

    getRangeLabel = function (page, pageSize, length) {
        const of = this.translate.instant('MatTablePaginator.Of');
        if (length === 0 || pageSize === 0) {
            return '0 ' + of + ' ' + length;
        }
        length = Math.max(length, 0);
        const startIndex = page * pageSize;
        // If the start index exceeds the list length, do not try and fix the end index to the end.
        const endIndex = startIndex < length ?
            Math.min(startIndex + pageSize, length) :
            startIndex + pageSize;
        return startIndex + 1 + ' - ' + endIndex + ' ' + of + ' ' + length;
    };

    injectTranslateService(translate: TranslateService) {
        this.translate = translate;

        this.translate.onLangChange.subscribe(() => {
            this.translateLabels();
        });

        this.translateLabels();
    }

    translateLabels() {
        this.firstPageLabel = this.translate.instant('MatTablePaginator.FirstPageLabel');
        this.lastPageLabel = this.translate.instant('MatTablePaginator.LastPageLabel');
        this.itemsPerPageLabel = this.translate.instant('MatTablePaginator.ItemsPerPageLabel');
        this.nextPageLabel = this.translate.instant('MatTablePaginator.NextPage');
        this.previousPageLabel = this.translate.instant('MatTablePaginator.PreviousPage');
    }
}