import { Component, Input, OnInit } from "@angular/core"
import { Router } from "@angular/router"
import { OrderService } from "src/app/services/order.service"

@Component({
	selector: "app-order-item",
	templateUrl: "./order-item.component.html",
	styleUrls: ["./order-item.component.css"],
})
export class OrderItemComponent implements OnInit {
	@Input("order") order = null
	@Input() isAdmin: boolean = false
	@Input() showActions: boolean = true

	constructor(private router: Router, private orderService: OrderService) {}

	ngOnInit(): void {}

	goToDetail() {
		this.orderService.selectedOrder = this.order
		this.router.navigate(["/admin/detail"])
	}
	goToMyDetail() {
		this.orderService.selectedOrder = this.order
		this.router.navigate(["/master/detail"])
	}
}
