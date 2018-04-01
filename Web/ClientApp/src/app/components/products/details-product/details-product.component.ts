import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductsService } from '../../../services/products.service';
import { IProduct } from '../../../models/product';
import { HttpErrorResponse } from '@angular/common/http';
import { SnackBarsServiceService } from '../../../services/snack-bars-service.service';

@Component({
  selector: 'app-details-product',
  templateUrl: './details-product.component.html',
  styleUrls: ['./details-product.component.css']
})
export class DetailsProductComponent implements OnInit {

  id: number;

  private sub: any;

  product: IProduct;

  constructor(
    private _route: ActivatedRoute,
    private _router: Router,
    private _productService: ProductsService,
    private _snackBarsService: SnackBarsServiceService) { }

  ngOnInit() {
    this.sub = this._route.params.subscribe(params => {
      this.id = +params['id'];
      if(this.id < 1){
        this._router.navigate(['/404']);
      }else{
        this._productService.getById(this.id).subscribe(
          result => this.product = result,
          error => this.showServerErrorSnackBar(error)
        )
      }
    });
  }

  showServerErrorSnackBar(error: HttpErrorResponse) {
    let errorString = 'Server error ' + error.status + ': ' + error.statusText;
    this._snackBarsService.showError(errorString)
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

}
