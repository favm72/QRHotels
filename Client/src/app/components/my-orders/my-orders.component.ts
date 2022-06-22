import { Component, OnDestroy, OnInit } from "@angular/core"
import { Subscription } from "rxjs"
import { NotificationService } from "src/app/services/notification.service"
import { OrderService } from "src/app/services/order.service"
import { SignalOrderService } from "src/app/services/signal-order.service"

@Component({
	selector: "app-my-orders",
	templateUrl: "./my-orders.component.html",
	styleUrls: ["./my-orders.component.css"],
})
export class MyOrdersComponent implements OnInit, OnDestroy {
	orders = []
	signalSub: Subscription = null

	constructor(private orderService: OrderService, private signalOrder: SignalOrderService, private notif: NotificationService) {}

	ngOnDestroy(): void {
		if (this.signalSub != null) this.signalSub.unsubscribe()
	}

	async loadOrders() {
		await this.orderService.getOrdersByClient()
		this.orders = this.orderService.orders
	}

	async ngOnInit(): Promise<void> {
		this.loadOrders()
		this.signalSub = this.signalOrder.onGetOrder.subscribe(data => {
			this.loadOrders()
		})
	}
}
