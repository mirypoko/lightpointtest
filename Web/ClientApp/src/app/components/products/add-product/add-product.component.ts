import { Component, OnInit } from '@angular/core';
import { IShop } from '../../../models/shop';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ShopsService } from '../../../services/shops.service';
import { SnackBarsServiceService } from '../../../services/snack-bars-service.service';
import { Router } from '@angular/router';
import { ProductsService } from '../../../services/products.service';
import { IProduct } from '../../../models/product';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.css']
})
export class AddProductComponent implements OnInit {

  errors: string[] = [];

  shops: Array<IShop>;

  form = new FormGroup({
    "name": new FormControl("", [Validators.required]),
    "description": new FormControl("", [Validators.required]),
    "shopId": new FormControl("", [Validators.required]),
    "imgUrl": new FormControl("", [Validators.required, Validators.pattern("^(https?://)?(www\\.)?([-a-z0-9]{1,63}\\.)*?[a-z0-9][-a-z0-9]{0,61}[a-z0-9]\\.[a-z]{2,6}(/[-\\w@\\+\\.,~#\\?&/=%]*)?$")]),//Validators.pattern("/^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$/")]),
  });

  constructor(
    private _shopsService: ShopsService,
    private _productsService: ProductsService,
    private _snackBarsService: SnackBarsServiceService,
    private _router: Router) { }

  ngOnInit() {
    this._shopsService.get().subscribe(
      items => this.shops = items,
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

    let product: IProduct = {
      description: formData.description,
      name: formData.name,
      shopId: formData.shopId,
      shop: null,
      imgUrl: formData.imgUrl,
      id: 0
    };

    this._productsService.post(product).subscribe(
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
    this._router.navigate(['/products']);
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
