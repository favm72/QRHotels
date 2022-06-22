import { Component, OnInit, ViewChild } from "@angular/core"
import { faPen, faTrash, faPlus } from "@fortawesome/free-solid-svg-icons"
import { InfoService } from "src/app/services/info.service"
import { NotificationService } from "src/app/services/notification.service"
import { ProductService } from "src/app/services/product.service"
import { ModalComponent } from "../utils/modal/modal.component"

class FilterModel {
	hotelCode: string = ""
	idProvider: number = 0
	idCategory: number = 0
}

class ProductModel {
	id: number = 0
	name: string = ""
	description: string = ""
	price: number = 0
	newCategory: boolean = false
	categoryName: string = ""
	active: boolean = true
	image: string = ""
	orderNo: number = 0
	imageLink: string = ""
	fileName: string = ""
	modifiers: string = "[]"
	hotelCode: string = ""
	idProvider: number = 0
	idCategory: number = 0
}

class ModifierModel {
	title: string = ""
	type: string = "radio"
	min: number = 1
	max: number = 1
	items: string = ""
}

@Component({
	selector: "app-product-manager",
	templateUrl: "./product-manager.component.html",
	styleUrls: ["./product-manager.component.css"],
})
export class ProductManagerComponent implements OnInit {
	removeIcon = faTrash
	editIcon = faPen
	addIcon = faPlus
	modifiers = []
	hotels = []
	providers = []
	categories = []
	allcategories = []
	products = []
	current: ProductModel = new ProductModel()
	modifierInput: ModifierModel = new ModifierModel()
	filter: FilterModel = new FilterModel()
	mode = "create"
	price: string = "0.00"

	@ViewChild("mModifier") mModifier: ModalComponent
	constructor(private prod: ProductService, private info: InfoService, private notif: NotificationService) {}

	setPrice() {
		const newprice = parseFloat(this.price)
		if (!isNaN(newprice) && newprice < 1000) {
			this.price = newprice.toFixed(2)
			this.current.price = parseFloat(this.price)
		} else {
			this.price = "0.00"
			this.current.price = 0.0
		}
	}

	async ngOnInit(): Promise<void> {
		this.hotels = await this.info.loadHotels()
		this.providers = await this.info.loadProviders()
		this.allcategories = await this.prod.allcategories()
	}

	async loadCategories(reset: boolean = true) {
		if (this.filter.idProvider == 0 || this.filter.hotelCode == "") {
			this.categories = []
		} else {
			this.categories = await this.prod.categories(this.filter)
		}
		if (reset) this.filter.idCategory = 0
	}

	clearProduct() {
		this.current = new ProductModel()
		this.modifiers = []
		this.price = "0.00"
	}
	clearModifier() {
		this.modifierInput = new ModifierModel()
	}
	async search() {
		this.products = await this.prod.search(this.filter)
	}
	async modify(product) {
		console.log(product)
		let result = await this.prod.findbyid(product)
		for (let k in result) this.current[k] = result[k]
		this.price = this.current.price.toFixed(2)
		this.modifiers = JSON.parse(result.modifiers ?? "[]")
		this.mode = "update"
	}
	switchActive() {
		this.current.active = !this.current.active
	}
	switchCategoryMode() {
		this.current.newCategory = !this.current.newCategory
	}
	cancelUpdate() {
		this.clearProduct()
		this.mode = "create"
	}
	removeModifier(index) {
		this.modifiers.splice(index, 1)
	}
	openModalModifiers() {
		this.mModifier.open()
	}
	addModifier() {
		try {
			if (this.modifierInput.title.trim() == "") throw new Error("Ingrese el Nombre del modificador")
			if (this.modifierInput.items.trim().length == 0) throw new Error("Ingrese las opciones del modificador")
			let optionsCount = this.modifierInput.items.trim().split("\n").length
			if (this.modifierInput.type == "radio" && optionsCount == 1) throw new Error("Ingrese más de una opción")
			if (this.modifierInput.type == "checkbox") {
				this.modifierInput.min = +this.modifierInput.min
				this.modifierInput.max = +this.modifierInput.max
				if (this.modifierInput.min > optionsCount) throw new Error("Mínimo no debe exceder la cantidad de opciones.")
				if (this.modifierInput.max > optionsCount) throw new Error("Máximono debe exceder la cantidad de opciones.")
				if (this.modifierInput.max < this.modifierInput.min) throw new Error("Ingrese valores válidos mínimo y máximo")
				if (this.modifierInput.max == 0) throw new Error("valor máximo no puede ser cero")
			}
		} catch (error) {
			this.notif.info(error.message)
			return
		}

		let toAdd: any = {}
		toAdd.title = this.modifierInput.title
		toAdd.type = this.modifierInput.type

		if (this.modifierInput.type == "radio") {
			toAdd.items = this.modifierInput.items.split("\n")
		}
		if (this.modifierInput.type == "checkbox") {
			toAdd.items = this.modifierInput.items.split("\n").map(x => ({ name: x, value: false }))
			toAdd.min = this.modifierInput.min
			toAdd.max = this.modifierInput.max
		}
		this.modifiers.push(toAdd)
		this.mModifier.close()
		this.clearModifier()
	}
	async update() {
		try {
			this.current.idProvider = this.filter.idProvider
			this.current.hotelCode = this.filter.hotelCode
			if (this.current.id == 0) throw new Error("Id inválido, inténtelo nuevamente")
			if (this.current.hotelCode == "") throw new Error("Seleccione un Hotel")
			if (this.current.idProvider == 0) throw new Error("Seleccione un outlet")
			if (this.current.idCategory == 0 && !this.current.newCategory) throw new Error("Seleccione una categoría")
			if ((this.current.name ?? "").trim() == "") throw new Error("Ingrese un nombre")
		} catch (error) {
			this.notif.info(error.message)
			return
		}
		this.current.orderNo = +this.current.orderNo
		this.current.modifiers = JSON.stringify(this.modifiers)

		await this.prod.update(this.current)
		await this.loadCategories(false)

		if (this.current.newCategory) {
			this.allcategories = await this.prod.allcategories()
		} else {
			await this.search()
		}
		this.clearProduct()
		this.mode = "create"
	}

	async create() {
		try {
			this.current.idProvider = this.filter.idProvider
			this.current.hotelCode = this.filter.hotelCode
			if (this.current.hotelCode == "") throw new Error("Seleccione un Hotel")
			if (this.current.idProvider == 0) throw new Error("Seleccione un outlet")
			if (this.current.idCategory == 0 && !this.current.newCategory) throw new Error("Seleccione una categoría")
			if ((this.current.name ?? "").trim() == "") throw new Error("Ingrese un nombre")
		} catch (error) {
			this.notif.info(error.message)
			return
		}
		this.current.orderNo = +this.current.orderNo
		this.current.modifiers = JSON.stringify(this.modifiers)

		await this.prod.create(this.current)
		await this.loadCategories(false)

		if (this.current.newCategory) {
			this.allcategories = await this.prod.allcategories()
		} else {
			await this.search()
		}
		this.clearProduct()
	}
}
