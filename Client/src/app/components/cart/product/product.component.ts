import { Component, Input, OnInit } from "@angular/core"
import { Router } from "@angular/router"
import { CartService } from "src/app/services/cart.service"
import { NotificationService } from "src/app/services/notification.service"

@Component({
	selector: "app-product",
	templateUrl: "./product.component.html",
	styleUrls: ["./product.component.css"],
})
export class ProductComponent implements OnInit {
	@Input("product") model = null
	@Input("category") cat = null
	showAddToCart = true

	constructor(private router: Router, private cart: CartService, private notif: NotificationService) {
		this.showAddToCart = !cart.viewOnly
	}

	ngOnInit(): void {}

	async addToCart(): Promise<void> {
		let verify = await this.cart.enabled(this.cart.provider)
		if (verify.data) {
			this.model.idcat = this.cat.id
			this.model.catname = this.cat.name
			this.cart.product = this.model
			this.router.navigate(["/addtocart"])
		} else {
			this.notif.info(verify.message)
			this.router.navigate(["/master/main"])
		}
	}
}
