import { Component, OnInit } from '@angular/core';
import { ShopsService } from '../../../services/shops.service';
import { HttpErrorResponse } from '@angular/common/http';
import { IShop } from '../../../models/shop';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { SnackBarsServiceService } from '../../../services/snack-bars-service.service';
import { Router } from '@angular/router';
import { IShopMode } from '../../../models/shopMode';

@Component({
  selector: 'app-add-shop',
  templateUrl: './add-shop.component.html',
  styleUrls: ['./add-shop.component.css']
})
export class AddShopComponent implements OnInit {

  errors: string[] = [];

  shopModes: Array<IShopMode>;

  form = new FormGroup({
    "name": new FormControl("", [Validators.required]),
    "address": new FormControl("", [Validators.required]),
    "shopModeId": new FormControl("", [Validators.required]),
  });

  constructor(
    private _shopService: ShopsService,
    private _snackBarsService: SnackBarsServiceService,
    private _router: Router) { }

  ngOnInit() {
    this._shopService.getShopMods().subscribe(
      items => this.shopModes = items,
      error => this.showServerErrorSnackBar(error)
    )
  }

  submit() {

    this.clearFormErrors();

    if (!this.form.valid) {
      this.showFormErrors(['Fill in required fields']);
      return;
    }

    let formData = this.form.value;

    let shop: IShop = {
      address: formData.address,
      name: formData.name,
      shopModeId: formData.shopModeId,
      shopMode: null,
      id: 0
    };

    this._shopService.post(shop).subscribe(
      result => {
        this.showSuccessSnackBar("Added")
        this.GoToShopList()
      },
      (error: HttpErrorResponse) => {
        if (error.status == 400) {
          this.errors = <Array<string>>error.error;
        } else {
          this.showServerErrorSnackBar(error);
        }
      }
    );
  }

  protected GoToShopList() {
    this._router.navigate(['/shops']);
  }

  protected showFormErrors(errors: Array<string>) {
    this.clearFormErrors();
    errors.forEach(element => {
      this.errors.push(element);
    });
  }

  protected clearFormErrors() {
    this.errors.splice(0, this.errors.length);
  }

  protected showServerErrorSnackBar(error: HttpErrorResponse) {
    let errorString = 'Server error ' + error.status + ': ' + error.statusText;
    this._snackBarsService.showError(errorString, true)
  }

  protected showSuccessSnackBar(message: string) {
    this._snackBarsService.showSuccess(message)
  }

}
