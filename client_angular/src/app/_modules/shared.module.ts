import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule } from 'ngx-toastr';

@NgModule({
    declarations: [],
    imports: [
        CommonModule,
        BsDropdownModule.forRoot(),
        ToastrModule.forRoot({
            positionClass: 'toast-bottom-right',
        }),
    ],
    exports: [BsDropdownModule, ToastrModule],
})
export class SharedModule {}
